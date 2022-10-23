using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ApiPerformance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RemoteServiceController : ControllerBase
    {
        /// <summary>
        /// 採用 非 同步程式設計 ，模擬需要花費 delay ms 時間，計算兩數值相加結果
        /// </summary>
        /// <param name="value1">數值1</param>
        /// <param name="value2">數值2</param>
        /// <param name="delay">模擬處理需要花費時間，單位為 ms (1ms = 0.001 秒)</param>
        /// <returns></returns>
        [HttpGet("AddAsync/{value1}/{value2}/{delay}")]
        public async Task<string> AddAsync(int value1, int value2, int delay)
        {
            // 紀錄開始執行這個 API 方法的開始時間
            DateTime Begin = DateTime.Now;

            #region 取得當前執行緒集區的狀態
            int workerThreadsAvailable;  // 可用的工作 Worker 執行緒
            int completionPortThreadsAvailable; // 可用的 IOCP I/O 完成埠 執行緒
            // 取得執行緒集區內可用的執行緒數值
            ThreadPool.GetAvailableThreads(out workerThreadsAvailable, out completionPortThreadsAvailable);
            #endregion

            // 模擬此次計算兩數值相加，需要耗費多少時間
            // 這裡使用 非同步 方式來進行等待、休息
            await Task.Delay(delay);

            // 計算兩數值相加的結果
            int sum = value1 + value2;

            // 紀錄完成結束執行這個 API 方法的開始時間
            DateTime Complete = DateTime.Now;
            string result= $"Result:{sum} " +
                $"AW:{workerThreadsAvailable} AC:{completionPortThreadsAvailable}" +
             $" Begin:{Begin:HH:mm:ss}({Begin:ss} - {Complete:ss} = {(Complete - Begin).TotalSeconds})";
            // 這行敘述將模擬將此次 API 呼叫記錄在伺服器上
            Console.WriteLine($"{result}");
            return result;
        }

        /// <summary>
        /// 採用 同步程式設計 ，模擬需要花費 delay ms 時間，計算兩數值相加結果
        /// </summary>
        /// <param name="value1">數值1</param>
        /// <param name="value2">數值2</param>
        /// <param name="delay">模擬處理需要花費時間，單位為 ms (1ms = 0.001 秒)</param>
        /// <returns></returns>
        [HttpGet("AddSync/{value1}/{value2}/{delay}")]
        public string AddSync(int value1, int value2, int delay)
        {
            // 紀錄開始執行這個 API 方法的開始時間
            DateTime Begin = DateTime.Now;

            #region 取得當前執行緒集區的狀態
            int workerThreadsAvailable;  // 可用的工作 Worker 執行緒
            int completionPortThreadsAvailable; // 可用的 IOCP I/O 完成埠 執行緒
            // 取得執行緒集區內可用的執行緒數值
            ThreadPool.GetAvailableThreads(out workerThreadsAvailable, out completionPortThreadsAvailable);
            #endregion

            // 模擬此次計算兩數值相加，需要耗費多少時間
            // 這裡使用 同步 方式來進行等待、休息
            Thread.Sleep(delay);

            // 計算兩數值相加的結果
            int sum = value1 + value2;

            // 紀錄完成結束執行這個 API 方法的開始時間
            DateTime Complete = DateTime.Now;
            string result = $"Result:{sum} " +
             $"AW:{workerThreadsAvailable} AC:{completionPortThreadsAvailable}" +
             $" ({Begin:ss} - {Complete:ss} = {(Complete - Begin).TotalSeconds})";
            // 這行敘述將模擬將此次 API 呼叫記錄在伺服器上
            Console.WriteLine($"{result}");
            return result;
        }

        /// <summary>
        /// 可以設定這台 Web API 主機上的執行緒集區隨著提出新要求，視需要建立的執行緒最小數目
        /// </summary>
        /// <param name="workerThreads">執行緒集區視需要建立的背景工作執行緒最小數目</param>
        /// <param name="completionPortThreads">執行緒集區視需要建立的非同步 I/O 執行緒最小數目</param>
        /// <returns></returns>
        [HttpGet("SetThreadPool/{workerThreads}/{completionPortThreads}")]
        public string SetThreadPool(int workerThreads, int completionPortThreads)
        {
            ThreadPool.SetMinThreads(workerThreads, completionPortThreads);

            string result = "OK";
            int workerThreadsAvailable;
            int completionPortThreadsAvailable;
            int workerThreadsMax;
            int completionPortThreadsMax;
            int workerThreadsMin;
            int completionPortThreadsMin;
            ThreadPool.GetAvailableThreads(out workerThreadsAvailable, out completionPortThreadsAvailable);
            ThreadPool.GetMaxThreads(out workerThreadsMax, out completionPortThreadsMax);
            ThreadPool.GetMinThreads(out workerThreadsMin, out completionPortThreadsMin);

            DateTime Complete = DateTime.Now;
            result = "OK " + $"AW:{workerThreadsAvailable} AC:{completionPortThreadsAvailable}" +
                $" MaxW:{workerThreadsMax} MaxC:{completionPortThreadsMax}" +
                $" MinW:{workerThreadsMin} MinC:{completionPortThreadsMin} ";

            return result;
        }

        /// <summary>
        /// 可以設定這台 Web API 主機上的可並行使用之執行緒集區的要求數目。 
        /// 超過該數目的所有要求會繼續佇列，直到可以使用執行緒集區執行緒為止
        /// </summary>
        /// <param name="workerThreads">執行緒集區中的背景工作執行緒最大數目</param>
        /// <param name="completionPortThreads">執行緒集區中的非同步 I/O 執行緒最大數目</param>
        /// <returns></returns>
        [HttpGet("SetMaxThreadPool/{workerThreads}/{completionPortThreads}")]
        public string SetMaxThreadPool(int workerThreads, int completionPortThreads)
        {
            ThreadPool.SetMaxThreads(workerThreads, completionPortThreads);

            string result = "OK";
            int workerThreadsAvailable;
            int completionPortThreadsAvailable;
            int workerThreadsMax;
            int completionPortThreadsMax;
            int workerThreadsMin;
            int completionPortThreadsMin;
            ThreadPool.GetAvailableThreads(out workerThreadsAvailable, out completionPortThreadsAvailable);
            ThreadPool.GetMaxThreads(out workerThreadsMax, out completionPortThreadsMax);
            ThreadPool.GetMinThreads(out workerThreadsMin, out completionPortThreadsMin);

            DateTime Complete = DateTime.Now;
            result = "OK " + $"AW:{workerThreadsAvailable} AC:{completionPortThreadsAvailable}" +
                $" MaxW:{workerThreadsMax} MaxC:{completionPortThreadsMax}" +
                $" MinW:{workerThreadsMin} MinC:{completionPortThreadsMin} ";

            return result;
        }

        /// <summary>
        /// 取得這台主機服務所使用的執行緒集區運行參數
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetThreadPool")]
        public string GetThreadPool()
        {
            string result = "";
            int workerThreadsAvailable;
            int completionPortThreadsAvailable;
            int workerThreadsMax;
            int completionPortThreadsMax;
            int workerThreadsMin;
            int completionPortThreadsMin;
            ThreadPool.GetAvailableThreads(out workerThreadsAvailable, out completionPortThreadsAvailable);
            ThreadPool.GetMaxThreads(out workerThreadsMax, out completionPortThreadsMax);
            ThreadPool.GetMinThreads(out workerThreadsMin, out completionPortThreadsMin);

            DateTime Complete = DateTime.Now;
            //var memoryMetrics = GetWindowsMetrics();
            result = "" +
                $" Processor:{Environment.ProcessorCount} " +
                $" AW:{workerThreadsAvailable} AC:{completionPortThreadsAvailable}" +
                $" MaxW:{workerThreadsMax} MaxC:{completionPortThreadsMax}" +
                $" MinW:{workerThreadsMin} MinC:{completionPortThreadsMin} ";

            return result;
        }

        /// <summary>
        /// 取得這台主機上的 API 服務所使用的記憶體用量狀況
        /// </summary>
        /// <returns></returns>
        private MemoryMetrics GetWindowsMetrics()
        {
            var output = "";

            var info = new ProcessStartInfo();
            info.FileName = "wmic";
            info.Arguments = "OS get FreePhysicalMemory,TotalVisibleMemorySize /Value";
            info.RedirectStandardOutput = true;

            using (var process = Process.Start(info))
            {
                output = process.StandardOutput.ReadToEnd();
            }

            var lines = output.Trim().Split("\n");
            var freeMemoryParts = lines[0].Split("=", StringSplitOptions.RemoveEmptyEntries);
            var totalMemoryParts = lines[1].Split("=", StringSplitOptions.RemoveEmptyEntries);

            var metrics = new MemoryMetrics();
            metrics.Total = Math.Round(double.Parse(totalMemoryParts[1]) / 1024, 0);
            metrics.Free = Math.Round(double.Parse(freeMemoryParts[1]) / 1024, 0);
            metrics.Used = metrics.Total - metrics.Free;

            return metrics;
        }
    }
    public class MemoryMetrics
    {
        public double Total;
        public double Used;
        public double Free;
    }
}

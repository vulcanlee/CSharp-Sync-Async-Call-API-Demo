# 查詢 Azure 上這台主機服務所使用的執行緒集區運行參數
## https://blazortw.azurewebsites.net/api/RemoteService/GetThreadPool

Processor:4  AW:1022 AC:1000 MaxW:1023 MaxC:1000 MinW:4 MinC:4 


# 可以設定這台 Web API 主機上的執行緒集區隨著提出新要求，視需要建立的執行緒最小數目
## https://blazortw.azurewebsites.net/api/RemoteService/SetThreadPool/250/250

"workerThreads">執行緒集區視需要建立的背景工作執行緒最小數目
"completionPortThreads">執行緒集區視需要建立的非同步 I/O 執行緒最小數目


# 可以設定這台 Web API 主機上的可並行使用之執行緒集區的要求數目。 

  >超過該數目的所有要求會繼續佇列，直到可以使用執行緒集區執行緒為止
  
## https://blazortw.azurewebsites.net/api/RemoteService/SetMaxThreadPool/250/250

"workerThreads" > 執行緒集區中的背景工作執行緒最大數目
"completionPortThreads" > 執行緒集區中的非同步 I/O 執行緒最大數目

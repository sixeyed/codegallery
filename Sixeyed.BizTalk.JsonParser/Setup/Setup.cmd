@echo off

@echo Copying pipeline component assembly...
copy Sixeyed.BizTalk.JsonParser.PipelineComponents.dll "C:\Program Files (x86)\Microsoft BizTalk Server 2013\Pipeline Components\"

@echo Creating drop locations...
md c:\Drops\VehicleSnapshotRequests
md c:\Drops\VehicleSnapshots

@echo Done! 
@echo To use: build and deploy the BizTalk sample solution; replace YOUR-DATABASE-NAME and YOUR-API-KEY in the bindings file and import into the sample application; put your document id in the sample .txt file; copy to c:\Drops\VehicleSnapshotRequests; the mapped XML response will be in c:\Drops\VehicleSnapshots.

pause


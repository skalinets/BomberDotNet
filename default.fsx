// include Fake libs
#I @"c:\fake\"
#r "FakeLib.dll"

open Fake

let buildDir = @".\output"
let solution = !+ @".\Bomberman\Bomberman.sln" |> Scan

Target "Build" (fun _ -> 

  MSBuildRelease buildDir "Build" solution
     |> Log "AppBuild-Output"
)

Target "Run" (fun _ ->
    let result = ExecProcess (fun info ->
        info.FileName <-  buildDir + "\Runner.exe"
        info.Arguments <- "") (System.TimeSpan.FromMinutes 20.) 
     
    if result <> 0 then failwith "cannot run runner"      
)

"Build" ==> "Run"

Run "Run"
namespace Review

module MainApp = 
    open System
    open CurriedFunctions           //open module

    [<EntryPoint>]                  //mark as entry function to a console application
    let main (argv) =               
        printfn "Hello World from F#!"
        let a = Basics.basicFeatures 1 2

        printfn "%s || %d" (Records.updatedRecord1.ToString()) composedResult
        Console.WriteLine("")
        let key = Console.ReadKey()
        0

namespace Review
open System

//namespace cannot have values so create module
module Basics =
    //no accessibility modifier, public is the default for top-level values
    //no static modifier, static is default in F#
    //let binds an immutable value/function to a symbol, let isn't var from C#
    let basicFeatures arg1 arg2 =           //no brackets for arguments means curried function of type: int -> int -> int
        
        let a = "a"                         //type inference, no type need to be specified
        let b:string = "b"                  //but type hints exist

        let add (a:int, b:int) : int =      //tupled function of type: int * int -> int
            let sum:int = a + b
            sum

        let r = System.Random()             //no new keyword, not used except for objects that implement IDisposable

        printfn "Hello: %d %s %d %s"
            (r.Next(1, 100)) a arg1 b       //no semicolons, newline is enough to detect where expression ends

        let sum = add(arg1, arg2)           //parentheses & commas needed for tupled arguments (unless only one defined)
        sum                                 //no return keyword


    //public by default but you can hide values using nested scopes: 
    let topLevelValue = //top-level scope
        let calculate(year) =   //nested function
            year - 1979

        let nestedValue =   //nested value
            calculate DateTime.Now.Year     //call
        sprintf "public value %d calculated in nested scope" nestedValue


    // Statement: never returns (void or returns unit), always has side-effect
    // Expression: always returns, rarely has side-effect
    let statement() =     //unit -> unit function
        ignore("do side-effect and keep calm")  // discard a value and return unit
        ()                                      // use unit explicitly


module Mutables = 
    open System.Net
    open System.Collections.Specialized;
    let mutableVariables = 
        let mutable variable = "temp"       //defining mutable variable
        variable <- "new-value"             //assign new value

        let wc = new WebClient()            //IDisposable so we use new keyword
        let myQuery = NameValueCollection()
        myQuery.Add("q", "query")
        wc.QueryString <- myQuery           //assign new value calling setter, wc is immutable but the object it refers to is mutable (as many others from BCL)

        new WebClient(QueryString = myQuery) //creating and mutating properties in one expression


module Tuples = 
    let parseToTuple(name:string) =
        let parts = name.Split(',')     
        parts.[0], parts.[1]      //creating two-part tuple

    let tabc = parseToTuple("a,b")
    let ta = fst tabc       //first of the two
    let tb = snd tabc       //second
    let shadowed = 
        //shadowing - repurpose a symbol multiple times within the same scope (except global scope)
        let ta, tb = tabc       //deconstructing tuple from value
        let ta, tb = parseToTuple("a,b,c")  //deconstructing tuple

        let nestedTuple = ("A", "B"), 0      //creating nested tuple of type: (string * string) * int
        let (tA, tB), tZ = nestedTuple   //deconstructing nested tuple
        let (tA, tB), _ = nestedTuple   //wildcard to discard
        ()


//type definition within namespace is allowed
type NestedRecord =  //creates constructor that requires all fields to be provided && public access for all fields && Equals()
    { A: string; B: string  //record may be defined in one-liner fashion
      C: string }    //or multiliner
module Records =
    type RecordType = 
        { D: string
          E: NestedRecord }
    let recordInstance1 =
        { D = "DD"; E = { A = "AA"; B = "BB"; C="CC" } }
    let updatedRecord1 = 
        { recordInstance1 with            //copy and update record instance
           D = "DD2" }
    let isSameAddress = (recordInstance1 = updatedRecord1)  //compare two records


module CurriedFunctions = 
    // A curried function is a function that itself returns a function. 
    // Partial application is the act of calling curried function to get back a new function
    let add (first:string) (second:int) = second + 2       //curried function with signature: string -> int -> int
    let addTwo = add "2"                       //apply partially to get back new function: int -> int
    let finalResult = addTwo 10                //apply second arg


    // curried functions also work well in pipelines:
    let pipeResult = 10 |> addTwo        //call function using the forward pipe operator
    let pipelinedSum = 50 |> add "0"        //pipeline function calls (aka thread-last clojure's macro)
                          |> add "0" 
                          |> addTwo
                          |> addTwo

    let composedAdd = addTwo >> addTwo >> addTwo //creating a function by by composing a set of functions together
    let composedResult = composedAdd(0)



module HigherOrderFunctions =
    let double (a:int) : int = 2 * a    //int -> int

    let calc (op:int->int) (num:int):int = op num       //higher-order function argument
    let doubledTwo = calc double 2                      //passing function
    let addedTwo = calc (fun a -> a + 2) 2              //passing lambda



module ListArraySeq =        //F# collections != LINQ
    let recordList = [ { A = "a1"; B = "b1"; C = "c1"}            //create list with [ ]
                       { A = "a2"; B = "b2"; C = "c2"} ]
    let listTransform = recordList |> List.map (fun k -> k.A)     //other useful: iter, collect, pairwise, groupBy, countBy, partition
                                   |> List.filter (fun k -> k.EndsWith "1")     //find, distinct, forall, exists

    let fromToList = [ 1 .. 6 ]                 //operator ..
    let front = 0 :: fromToList                 //place 0 at the front of list
    let head :: tail = front                    //head-tail split
    let appendList = fromToList @ [ 7 .. 9 ]    //add elements to new list
    
    let numbers = [ 1.0 .. 10.0 ]
    let sumAgg = numbers |> List.sum               //aggregates- take a collection of items and merge them into a smaller one
    let avgAgg = numbers |> List.average           //others: List.max, List.min

    let numArray = [| 1; 2; 3; 4; 6 |]  //create array with [| |]
    let firstNumber = numArray.[0]      //access by index
    let slice = numArray.[0 .. 2]       //slicing
    numArray.[0] <- 99                  //mutate first

    let convertToQeq = fromToList |> List.toArray |> Seq.ofArray        //conversion


 
 module MapSet = 
    open System.Collections.Generic                 //mutable dictionary for thousands of additions or removals
    let strictMap = Dictionary<string, float>()     //strict typed
    let genericMap = Dictionary<_,_>()              //generic typed with placeholders
    let genericMap2 = Dictionary()                  //omitting generic types completely
    strictMap.Add("AAA", 0.33)
    genericMap.Add("AAA", 333)
    genericMap2.Add("AAA", 333)

    let immutableDict = [ "A", 0.1; "B", 0.2; "C", 0.3 ] |> dict        //use dict to convert list of tuples to immutable IDictionary

    let immutableMap = [ "A", 0.1; "B", 0.2; "C", 0.3 ] |> Map.ofList   //convert list to immutable Map for lookup purposes (reach functions eco)
    let valueOfA = immutableMap.["A"]
    let newMap = immutableMap |> Map.add "D" 0.4            //copy map and add element
                              |> Map.remove "A"             //copy map without element

    let setFromList = [ "A"; "B"; "C" ] |> Set.ofList       //create set from list
    let setFromDistinct = [ "A"; "B"; "C"; "C" ] |> List.distinct       //other useful ops: @ + - Set.intersect Set.isSubset



module Folding =
    let sum inputs =
        Seq.fold
            (fun state input -> state + input)      //folder function doing sum
            0                                       //initial value
            inputs                                  //input collection

    let input = [ 1 .. 5 ]
    let foldedSum = sum input

    let foldedSum2 = input |> Seq.fold (fun state input -> state + input) 0         //call using pipeline
    let foldedSum3 = (0, input) ||> Seq.fold (fun state input -> state + input)     //call using double pipeline- move both arguments

    //other fold related functions: foldBack, mapFold, reduce, scan, unfold


module Loops = 
    for number in 1 .. 10 do printfn "%d" number             //counting up
    for number in 10 .. -1 .. 1 do printfn "%d" number       //counting down
    for number in [ 1; 2; 3 ] do printfn "%d" number         //foreach
    for even in 2 .. 2 .. 10 do printfn "%d" even            //range with custom step

    open System.IO
    let readFile arg = 
        let reader = new StreamReader(File.OpenRead @"file.txt")
        while (not reader.EndOfStream) do                           //while loop
            printfn "%s" (reader.ReadLine())
        
    let listOfSquares = [ for i in 1 .. 10 -> i * i ]               //for comprehension


module Branching =
    let ifBranching num =
        if num < 10 then 500
        elif num < 5 then 100
        else 1

    let patternMatching (obj:NestedRecord) : int = 
        match obj with
        | { A = "a"; B = "b"; C = "c" } -> 100              //record matching, also guards & nested matches & collections matching available
        | { A = "d" }  -> 50
        | _ -> 0
    let res = patternMatching { A = "a"; B = "b"; C = "c" }


module OtherTypes = 

    //enum
    type SateEnum = 
    | Stop = 0
    | Start = 1
    | Running = 2


    //option
    let someNumber : int option = Some 10
    let optionMatching score = 
        match score with
        | Some 0 -> 10
        | Some 1 -> 100
        | Some s -> s
        | None -> 0
    let res = optionMatching(someNumber)


    //Discriminated unions, modeling is-a relationships (aka inheritance in FP)
    type BaseType =
    | DerivedType1 of Field1:int * Field2:int
    | DerivedType2
    | DerivedType3 of Field3:int

    let type1Instance:BaseType = DerivedType1(Field1 = 1, Field2 = 2)
    //let type1Instance:DerivedType1 = DerivedType1(Field1 = 1, Field2 = 2)             //the type DerivedType1 is not defined
    //let type1Instance:BaseType.DerivedType1 = DerivedType1(Field1 = 1, Field2 = 2)    //the type DerivedType1 is not defined

    let type2Instance = DerivedType2
    let type3Instance = DerivedType3 1

    let aTasteOfPolimorphicFunction obj = 
        match obj with
        | DerivedType1(f1, f2) -> sprintf "Calling function for DerivedType1 %d %d" f1 f2 
        | DerivedType2 -> sprintf "Calling function for DerivedType2"
        | DerivedType3 _ -> sprintf "Calling function for DerivedType3"

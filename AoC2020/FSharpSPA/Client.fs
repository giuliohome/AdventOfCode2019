namespace FSharpSPA

open WebSharper
open WebSharper.JavaScript
open WebSharper.JQuery
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Templating

[<JavaScript>]
module Client =
    open WebSharper.UI.Html
    open WebSharper.UI.Client
    open WebSharper.JQuery

    // The templates are loaded from the DOM, so you just can edit index.html
    // and refresh your browser, no need to recompile unless you add or remove holes.
    type IndexTemplate = Template<"index.html", ClientLoad.FromDocument>

    let People =
        ListModel.FromSeq [
            "John"
            "Paul"
        ]
    
    let AoC = 
        div [] [
            h1 [] [text "Let's start with Advent of Code"]
            p [] [text "Quick test"]
            Doc.Button "Grab input day 1 part 1 2019" [] (fun () ->
                let settings = AjaxSettings()
                settings.BeforeSend <-
                        fun (req : JqXHR) (_: AjaxSettings) -> 
                            req.SetRequestHeader("crossDomain","")
                settings.CrossDomain <- true
                settings.Success <- fun data  _  _ ->
                        let data = As<string> data
                        JQuery.Of("#inputText").Val(data).Ignore

                JQuery.Ajax("Content/test_input.txt",settings )
                    .Done(
                        fun () -> Console.Log("Done")
                    ) 
                    |> ignore
            )
            textarea [attr.id "inputText";] []
        ]

    [<SPAEntryPoint>]
    let Main () =
        IndexTemplate.AoC2020()
            .QuickTest(AoC)
            .Doc()
        |> Doc.RunById "aoc"
        let newName = Var.Create ""
        IndexTemplate.Main()
            .ListContainer(
                People.View.DocSeqCached(fun (name: string) ->
                    IndexTemplate.ListItem().Name(name).Doc()
                )
            )
            .Name(newName)
            .Add(fun _ ->
                People.Add(newName.Value)
                newName.Value <- ""
            )

            .Doc()
        |> Doc.RunById "main"

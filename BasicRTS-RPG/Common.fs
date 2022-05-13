module Common

let countProfession population targetType = List.where (fun person -> person = targetType) population |> List.length 

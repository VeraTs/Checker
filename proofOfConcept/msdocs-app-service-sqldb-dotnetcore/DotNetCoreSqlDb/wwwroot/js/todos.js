"use strict";

//document.getElementById("sendButton").style.backgroundColor = "red";
//document.getElementById("todosList").style.backgroundColor = "red";

var connection = new signalR.HubConnectionBuilder().withUrl("/todosHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveList", function (desc, date) {
    var li = document.createElement("li");
    document.getElementById("todosList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${date} : ${desc}`;
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
    connection.invoke("InitialToDos");
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", async function (event) {
    var desc = document.getElementById("desc").value;
    var date = document.getElementById("date").value;
    try {
        connection.invoke("AddToDo", desc, date);
        event.preventDefault();
    }catch(err){
        console.error(err);
    }
});
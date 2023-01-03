var messageInput = document.getElementById('messageInput');
const queryString = window.location.search;
const urlParams = new URLSearchParams(queryString);
var name = urlParams.get('name');
var access_token = localStorage.getItem(name);
var LoadName = document.getElementById("FirstLoad");
var friend = document.getElementById("friend");
console.log(access_token + " token");
var list = document.getElementById("messagesList");
var sendto = "";
const LtFriends = [];
//check token exsits
if (access_token == null) {
    window.location.href = "https://localhost:44377/login"
}
messageInput.focus();

//create connection
var connection = new signalR.HubConnectionBuilder().withUrl('http://localhost:4001/hubs/MessHub', { accessTokenFactory: () => localStorage.getItem(name) })
    .build();
//receive message from all user 
connection.on("Message", function (user, message) {
    if (user == name) {
        user = "You";
        list.innerHTML = list.innerHTML + "<div class='chat-message-right'>"
            + "<div class='card-body'>"
            + "<h6 class='card-subtitle float-right'>" + `${user}` + "</h6><br>"
            + "<p class='card-text float-right'>"
            + "<div class='flex-shrink-1 bg-light rounded'>" + `${message}` + "</div>"
            + "</p>"
            + "</div > "
            + "</div>";
    }
    else {
        list.innerHTML = list.innerHTML + "<div class='chat-message-left'>"
            + "<div class='card-body'>"
            + "<h6 class='card-subtitle float-left'>" + `${user}` + "</h6><br>"
            + "<p class='card-text float-left'>"
            + "<div class='flex-shrink-1 bg-light rounded'>" + `${message}` + "</div>"
            + "</p>"
            + "</div > "
            + "</div>";
    }
});

//receive message from specify user 
connection.on("SpecifyMessage", function (message) {
        list.innerHTML = list.innerHTML + "<div class='chat-message-left'>"
            + "<div class='card-body'>"
            + "<h6 class='card-subtitle float-left'>" + sendto + "</h6><br>"
            + "<p class='card-text float-left'>"
            + "<div class='flex-shrink-1 bg-light rounded'>" + `${message}` + "</div>"
            + "</p>"
            + "</div > "
            + "</div>";
    
});

// receive listfriend 
connection.on("FriendList", function (Friends) {
    listUser = JSON.parse(Friends);
    console.log(listUser);
    listUser.forEach(obj => {
        Object.entries(obj).forEach(([key, value]) => {
            if (key == "Username") {
                LtFriends.push(value);
                displayF(value);
                
            }
        });
    });
});
//a friend online
connection.on("goOnline", function (name) {
    if (LtFriends.includes(name)) {
        var status = document.getElementById("status_" + name);
        status.innerHTML = "Online";
    }
});

//a friend offline
connection.on("goOffline", function (name) {
    if (LtFriends.includes(name)) {
        var status = document.getElementById("status_" + name);
        status.innerHTML = "Offline";
    }
});
//start connect and call getFriend 
async function start() {
    try {
        console.log("SignalR Connecting...");
        await connection.start();
        console.log("SignalR Connected.");
        await connection.invoke("getFriend", name);
    } catch (err) {
        console.log(err);
    }
};
// event for button send
document.getElementById("sendButton").addEventListener("click", function (event) {
    if (messageInput.value != null && messageInput.value != "") {
        if (sendto != null && sendto != "") {
            list.innerHTML = list.innerHTML + "<div class='chat-message-right'>"
                + "<div class='card-body'>"
                + "<h6 class='card-subtitle float-right'>You</h6><br>"
                + "<p class='card-text float-right'>"
                + "<div class='flex-shrink-1 bg-light rounded'>" + messageInput.value + "</div>"
                + "</p>"
                + "</div > "
                + "</div>";
            connection.invoke("SendPrivateMessage", sendto, messageInput.value);
        }
        else {
            connection.invoke("Send", name, messageInput.value);
        }
        messageInput.value = "";
    }
    event.preventDefault();
});
//event for enter
messageInput.addEventListener("keypress", function (event) {
    if (event.key == "Enter") {
        event.preventDefault();
        document.getElementById("sendButton").click();
    }

})
// show friendList
function displayF(user)
{
    var userList = document.getElementById("userList");
    userList.innerHTML = userList.innerHTML + "<div class='list-group-item list-group-item-action border-0' id=" + `${user}` + " onclick ='changeName(this)' >"
        + "<div class='badge bg-success float-right'></div>"
        + "<div class='d-flex align-items-start'>"
        + "<img src='https://bootdey.com/img/Content/avatar/avatar5.png' class='rounded-circle mr-1' alt='Vanessa Tucker' width='40' height='40'>"
        + "<div class ='flex-grow-1 ml-3'>"
        + `${user}`
        + "<div class='small' id= 'status_" + `${user}` + "' > Offline </div>"
        + "</div>"
        + "</div>"
        + "</div>";
}
// onclick friend
function changeName(item) {
    LoadName.innerHTML = item.id;
    sendto = item.id;
    list.innerHTML = "";
    //gethistorychat()
} 
start();
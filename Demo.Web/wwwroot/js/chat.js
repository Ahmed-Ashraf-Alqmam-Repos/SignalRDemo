const connection = new signalR.HubConnectionBuilder()
    .withUrl('/chatHub')
    .build();

connection.on('ReceiveMessage', (user, message) => {

    const data = `${user} : ${message}`;

    const li = document.createElement('li');
    li.textContent = data;

    const messageList = document.getElementById('messagesList');
    messageList.appendChild(li);
});

const sendButton = document.getElementById('sendButton');

sendButton.addEventListener('click', event => {

    const user = document.getElementById('userInput').value;
    const message = document.getElementById('messageInput').value;

    connection.invoke("SendMessage", user, message)
        .catch(error => console.log(error));

    event.preventDefault();
});

connection.start().catch(error => console.log(error));
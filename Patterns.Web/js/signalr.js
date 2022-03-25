const signalR = require("@microsoft/signalr");
const { render } = require('./render');

const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl('/patterns')
    .withAutomaticReconnect()
    .build();

hubConnection.on('NewCells', cells => {
    render(cells);
});

export async function connect() {
    await hubConnection.start();
}

export async function start() {
    await hubConnection.invoke('Start', 'Life', {'width': '80', 'height': '60', 'Delay': '80'});
}

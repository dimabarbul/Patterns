const signalR = require("@microsoft/signalr");
const { render, clear } = require('./render');
const { get } = require('./controls');

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
    const args = get();

    await hubConnection.invoke(
        'Start',
        args.algorithm,
        args);
}

export async function stop() {
    await hubConnection.invoke('Stop');

    clear();
}

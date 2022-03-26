import * as signalR from '@microsoft/signalr';
import { render, clear } from './render';
import { get, State } from './controls';

const hubConnection: signalR.HubConnection = new signalR.HubConnectionBuilder()
    .withUrl('/patterns')
    .withAutomaticReconnect()
    .build();

let isRenderInProgress: boolean = false;

hubConnection.on('NewCells', (width: number, height: number, cells: number[]) => {
    if (isRenderInProgress) {
        return;
    }

    isRenderInProgress = true;
    render(width, height, cells);
    isRenderInProgress = false;
});

export async function connect(): Promise<void> {
    await hubConnection.start();
}

export async function start(): Promise<void> {
    const args: State = get();
    const delay: number = parseInt(args.delay);

    delete args.delay;

    await hubConnection.invoke(
        'Start',
        args.algorithm,
        delay,
        args);
}

export async function stop(): Promise<void> {
    await hubConnection.invoke('Stop');

    clear();
}

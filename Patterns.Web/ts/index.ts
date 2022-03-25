import * as signalR from './signalr';
import { init } from './controls';

window.onload = async function() {
    await signalR.connect();
    init();
};

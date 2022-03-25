const signalR = require('./signalr');

window.onload = async function() {
    await signalR.connect();
    await signalR.start();
};

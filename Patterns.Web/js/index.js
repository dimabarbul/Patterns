const signalR = require('./signalr');
const { init } = require('./controls');

window.onload = async function() {
    await signalR.connect();
    init();
};

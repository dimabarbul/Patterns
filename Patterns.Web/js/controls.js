const signalR = require('./signalr');

const startForm = document.getElementById('startForm');
const stopForm = document.getElementById('stopForm');
const algorithmInput = document.getElementById('algorithm');
const delayInput = document.getElementById('delay');
const widthInput = document.getElementById('width');
const heightInput = document.getElementById('height');

function save() {
    localStorage.setItem('state', JSON.stringify(get()));
}

function restore() {
    let stateJson = localStorage.getItem('state');
    if (stateJson === null) {
        return;
    }

    const state = JSON.parse(stateJson);
    algorithmInput.value = state.algorithm;
    delayInput.value = state.delay;
    widthInput.value = state.width;
    heightInput.value = state.height;
}

function toggleForm(form, isEnabled) {
    for (let element of form.elements) {
        if (isEnabled) {
            element.removeAttribute('disabled');
        } else {
            element.setAttribute('disabled', 'disabled');
        }
    }
}

export function init() {
    restore();
    toggleForm(startForm, true);
    toggleForm(stopForm, false);
}

export function get() {
    return {
        algorithm: algorithmInput.value,
        delay: delayInput.value,
        width: widthInput.value,
        height: heightInput.value,
    };
}

startForm.onsubmit = async function(e) {
    e.preventDefault();

    toggleForm(startForm, false);
    toggleForm(stopForm, true);

    save();
    await signalR.start();

    return false;
}

stopForm.onsubmit = async function(e) {
    e.preventDefault();

    toggleForm(startForm, true);
    toggleForm(stopForm, false);

    await signalR.stop();

    return false;
}

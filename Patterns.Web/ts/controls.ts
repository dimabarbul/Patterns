const signalR = require('./signalr');

const startForm: HTMLFormElement = document.getElementById('startForm') as HTMLFormElement;
const stopForm: HTMLFormElement = document.getElementById('stopForm') as HTMLFormElement;
const algorithmInput: HTMLSelectElement = document.getElementById('algorithm') as HTMLSelectElement;
const delayInput: HTMLInputElement = document.getElementById('delay') as HTMLInputElement;
const widthInput: HTMLInputElement = document.getElementById('width') as HTMLInputElement;
const heightInput: HTMLInputElement = document.getElementById('height') as HTMLInputElement;
const sizeInput: HTMLInputElement = document.getElementById('size') as HTMLInputElement;

export interface State {
    algorithm: string;
    delay: string;
    width: string;
    height: string;
    size: string;
}

function save(): void {
    localStorage.setItem('state', JSON.stringify(get()));
}

function restore(): void {
    let stateJson: string|null = localStorage.getItem('state');
    if (stateJson === null) {
        return;
    }

    const state: State = JSON.parse(stateJson) as State;
    algorithmInput.value = state.algorithm;
    delayInput.value = state.delay;
    widthInput.value = state.width;
    heightInput.value = state.height;
    sizeInput.value = state.size;
}

function toggleForm(form: HTMLFormElement, isEnabled: boolean): void {
    for (let i = 0; i < form.elements.length; i++) {
        if (isEnabled) {
            form.elements[i].removeAttribute('disabled');
        } else {
            form.elements[i].setAttribute('disabled', 'disabled');
        }
    }
}

export function init(): void {
    restore();
    toggleForm(startForm, true);
    toggleForm(stopForm, false);
}

export function get(): State {
    return {
        algorithm: algorithmInput.value,
        delay: delayInput.value,
        width: widthInput.value,
        height: heightInput.value,
        size: sizeInput.value,
    };
}

startForm.onsubmit = async function(e): Promise<boolean> {
    e.preventDefault();

    toggleForm(startForm, false);
    toggleForm(stopForm, true);

    save();
    await signalR.start();

    return false;
}

stopForm.onsubmit = async function(e): Promise<boolean> {
    e.preventDefault();

    toggleForm(startForm, true);
    toggleForm(stopForm, false);

    await signalR.stop();

    return false;
}

const signalR = require('./signalr');

const startForm: HTMLFormElement = document.getElementById('startForm') as HTMLFormElement;
const stopForm: HTMLFormElement = document.getElementById('stopForm') as HTMLFormElement;
const algorithmInput: HTMLSelectElement = document.getElementById('algorithm') as HTMLSelectElement;
const canvas: HTMLCanvasElement = document.getElementById('canvas') as HTMLCanvasElement;

export type State = { [key: string]: string };

function save(): void {
    const state: State = get();
    localStorage.setItem('state', JSON.stringify(state));

    canvas.width = parseInt(state.width) * parseInt(state.size);
    canvas.height = parseInt(state.height) * parseInt(state.size);
}

function restore(): void {
    let stateJson: string|null = localStorage.getItem('state');
    if (stateJson === null) {
        return;
    }

    const state: State = JSON.parse(stateJson) as State;

    for (let i = 0; i < startForm.elements.length; i++) {
        if (startForm.elements[i].id in state) {
            (startForm.elements[i] as HTMLFormElement).value = state[startForm.elements[i].id];
        }
    }
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
    hideCustomArgs();
    toggleForm(startForm, true);
    toggleForm(stopForm, false);
}

export function get(): State {
    let result: State = {};

    for (let i = 0; i < startForm.elements.length; i++) {
        if (startForm.elements[i].id) {
            result[startForm.elements[i].id] = (startForm.elements[i] as HTMLFormElement).value;
        }
    }

    return result;
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

function hideCustomArgs() {
    for (let i = 0; i < startForm.elements.length; i++) {
        if (startForm.elements[i].tagName.toLowerCase() === 'fieldset') {
            if (startForm.elements[i].id === algorithmInput.value) {
                startForm.elements[i].removeAttribute('disabled');
                startForm.elements[i].classList.remove('hidden');
            } else {
                startForm.elements[i].setAttribute('disabled', 'disabled');
                startForm.elements[i].classList.add('hidden');
            }
        }
    }
}

algorithmInput.onchange = function(e): void {
    hideCustomArgs();
}

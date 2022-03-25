import { get, State } from "./controls";

const canvas: HTMLCanvasElement = document.getElementById('canvas') as HTMLCanvasElement;
const context: CanvasRenderingContext2D = canvas.getContext('2d')
    || (() => { throw new Error('Canvas context is not available.'); })();

function displayCell(cell: string, column: number, row: number, size: number): void {
    context.fillStyle = cell;
    context.fillRect(column * size, row * size, size, size);
}

export function render(cells: string[][]): void {
    const state: State = get();

    for (let i = 0; i < cells.length; i++) {
        for (let j = 0; j < cells[i].length; j++) {
            displayCell(cells[i][j], i, j, parseInt(state.size));
        }
    }
}

export function clear(): void {
    context.clearRect(0, 0, canvas.width, canvas.height);
}

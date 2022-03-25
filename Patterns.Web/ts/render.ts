const canvas: HTMLCanvasElement = document.getElementById('canvas') as HTMLCanvasElement;
const context: CanvasRenderingContext2D = canvas.getContext('2d')
    || (() => { throw new Error('Canvas context is not available.'); })();

function displayCell(cell: string, column: number, row: number): void {
    context.fillStyle = cell;
    context.fillRect(column * 10, row * 10, 10, 10);
}

export function render(cells: string[][]): void {
    for (let i = 0; i < cells.length; i++) {
        for (let j = 0; j < cells[i].length; j++) {
            displayCell(cells[i][j], i, j);
        }
    }
}

export function clear(): void {
    context.clearRect(0, 0, canvas.width, canvas.height);
}

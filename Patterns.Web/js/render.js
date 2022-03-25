const canvas = document.getElementById('canvas');
const context = canvas.getContext('2d');

function displayCell(cell, column, row) {
    context.fillStyle = cell;
    context.fillRect(column * 10, row * 10, 10, 10);
}

export function render(cells) {
    for (let i = 0; i < cells.length; i++) {
        for (let j = 0; j < cells[i].length; j++) {
            displayCell(cells[i][j], i, j);
        }
    }
}

export function clear() {
    context.clearRect(0, 0, canvas.width, canvas.height);
}

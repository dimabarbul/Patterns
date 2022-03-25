const canvas = document.getElementById('canvas');

function displayCell(context, cell, column, row) {
    context.fillStyle = cell;
    context.fillRect(column * 10, row * 10, 10, 10);
}

export function render(cells) {
    const context = canvas.getContext('2d');
    for (let i = 0; i < cells.length; i++) {
        for (let j = 0; j < cells[i].length; j++) {
            displayCell(context, cells[i][j], i, j);
        }
    }
}

const fs = require('fs');
const path = require('path');

const jsPath = path.join(__dirname, '../../wwwroot/configurador/static/js');
const cssPath = path.join(__dirname, '../../wwwroot/configurador/static/css');

const renameFile = (dir, newName) => {
    if (!fs.existsSync(dir)) {
        console.log(`Directory not found: ${dir}. Skipping rename.`);
        return;
    }

    const files = fs.readdirSync(dir);
    const fileToRename = files.find(f => f.startsWith('main.') && (f.endsWith('.js') || f.endsWith('.css')));
    
    if (fileToRename) {
        const oldPath = path.join(dir, fileToRename);
        const newPath = path.join(dir, newName);
        fs.renameSync(oldPath, newPath);
        console.log(`Renamed ${oldPath} to ${newPath}`);
    } else {
        console.log(`No file starting with "main." found in ${dir}`);
    }
};

renameFile(jsPath, 'main.js');
renameFile(cssPath, 'main.css');

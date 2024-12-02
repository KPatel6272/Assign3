const sqlite3 = require('sqlite3').verbose();

const db = new sqlite3.Database('./database/greetings.db', (err) => {
    if (err) {
        console.error("Error opening database", err);
    } else {
        console.log("Connected to SQLite database.");
    }
});

module.exports = db;
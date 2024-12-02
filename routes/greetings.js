const express = require('express');
const db = require('../database/db');

const router = express.Router();

// Endpoint: Greet
router.post('/greet', (req, res) => {
    const { timeOfDay, language, tone } = req.body;

    db.get(
        `SELECT greetingMessage FROM Greetings WHERE timeOfDay = ? AND language = ? AND tone = ?`,
        [timeOfDay, language, tone],
        (err, row) => {
            if (err) {
                res.status(500).json({ error: "Database error" });
            } else if (!row) {
                res.status(404).json({ error: "Greeting not found" });
            } else {
                res.json({ greetingMessage: row.greetingMessage });
            }
        }
    );
});

// Endpoint: GetAllTimesOfDay
router.get('/times-of-day', (req, res) => {
    db.all(`SELECT DISTINCT timeOfDay FROM Greetings`, (err, rows) => {
        if (err) {
            res.status(500).json({ error: "Database error" });
        } else {
            res.json(rows.map(row => row.timeOfDay));
        }
    });
});

// Endpoint: GetSupportedLanguages
router.get('/languages', (req, res) => {
    db.all(`SELECT DISTINCT language FROM Greetings`, (err, rows) => {
        if (err) {
            res.status(500).json({ error: "Database error" });
        } else {
            res.json(rows.map(row => row.language));
        }
    });
});

module.exports = router;

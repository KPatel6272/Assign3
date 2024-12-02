const db = require('./db');

const seedDatabase = () => {
    db.serialize(() => {
        db.run(`CREATE TABLE IF NOT EXISTS Greetings (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            timeOfDay TEXT,
            language TEXT,
            greetingMessage TEXT,
            tone TEXT
        )`);

        db.run(`DELETE FROM Greetings`); // Clear old data

        const insertGreeting = db.prepare(`INSERT INTO Greetings (timeOfDay, language, greetingMessage, tone) VALUES (?, ?, ?, ?)`);
        insertGreeting.run("Morning", "English", "Good morning!", "Formal");
        insertGreeting.run("Morning", "English", "Morning!", "Casual");
        insertGreeting.run("Afternoon", "French", "Bon après-midi!", "Formal");
        insertGreeting.run("Evening", "Spanish", "¡Buenas noches!", "Formal");
        insertGreeting.finalize();

        console.log("Database seeded successfully!");
    });
};

seedDatabase();

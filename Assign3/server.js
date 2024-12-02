const express = require('express');
const bodyParser = require('body-parser');
const greetingsRoutes = require('./routes/greetings');

const app = express();
const PORT = 3000;

app.use(bodyParser.json());
app.use('/api', greetingsRoutes);

app.listen(PORT, () => {
    console.log(`Server is running on http://localhost:${PORT}`);
});

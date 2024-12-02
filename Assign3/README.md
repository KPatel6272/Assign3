
 
 # Node.js API with SQLite

## API Endpoints:

### 1. `/greet`
**POST** - Request a greeting based on time of day, language, and tone.
- Example:
  ```json
  {
    "timeOfDay": "Morning",
    "language": "English",
    "tone": "Formal"
  }
{
  "greetingMessage": "Good Morning!"
}


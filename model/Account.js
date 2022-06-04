const mongoose = require('mongoose');
const {Schema} = mongoose;

const userSchema = new Schema({
    username: String,
    password: String,
    name: String,
    surname1: String,
    surname2: String,
    birthDate: Date,
    sex: String,
    height: String,
    weight: String,
    state: String,
    attributes: [String],
    mail: String,
    phone: String,
    companion: Boolean,
    dailyExercise: Boolean,
    expertPatient: Boolean,
    companionAccess: Boolean,

    salt: String,
    lastAuth: Date,
});

mongoose.model('users', userSchema);
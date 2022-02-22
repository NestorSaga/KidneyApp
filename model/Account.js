const mongoose = require('mongoose');
const {Schema} = mongoose;

const accountSchema = new Schema({ // TODO añadir toda la informacion
    username: String,
    password: String,
    salt: String,

    lastAuth: Date,
});

mongoose.model('users', accountSchema);
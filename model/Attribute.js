const mongoose = require('mongoose');
const {Schema} = mongoose;

const attributeSchema = new Schema({ // TODO añadir toda la informacion
    name: {
        en: String,
        es: String
    },
    description: {
        en: String,
        es: String
    }
});

mongoose.model('attributes', attributeSchema);
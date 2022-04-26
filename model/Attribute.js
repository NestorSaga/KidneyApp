const mongoose = require('mongoose');
const {Schema} = mongoose;

const attributeSchema = new Schema({ 

    name: String,
    
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
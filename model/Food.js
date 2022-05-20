const mongoose = require('mongoose');
const {Schema} = mongoose;

const foodSchema = new Schema({
    meat: Array,
    fish: Array,
    deliMeats: Array,
    dairy: Array


});

mongoose.model('foods', foodSchema);
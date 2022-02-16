const express = require ("express");
const keys = require('./config/keys.js');
const app = express();

// Set up DB
const mongoose = require('mongoose');
const res = require("express/lib/response");
const req = require("express/lib/request");
mongoose.connect(keys.mongoURI);

//Set up DB models
require('./model/Account');

//Setup routes
require('./routes/authenticationRoutes')(app);

app.listen(keys.port, () => {
    console.log("Listening port " + keys.port);
});
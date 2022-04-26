const express = require ("express");
const keys = require('./config/keys.js');
const app = express();
const bodyParser = require('body-parser');

// parser
app.use(bodyParser.urlencoded({ extended: false }))

// Set up DB
const mongoose = require('mongoose');
const res = require("express/lib/response");
const req = require("express/lib/request");
mongoose.connect(keys.mongoURI);

//Set up DB models
require('./model/Account');
require('./model/Attribute');
require('./model/Achievement');
require('./model/Video');
require('./model/UserAchievement');
require('./model/VideoRegistry');

//Setup routes
require('./routes/dataRoutes')(app);
require('./routes/authenticationRoutes')(app);
require('./routes/achievementsRoutes')(app);



app.listen(keys.port, () => {
    console.log("Listening port " + keys.port);
});
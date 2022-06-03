const mongoose = require('mongoose');
const Account = mongoose.model('users');
const Food = mongoose.model('foods');
const Menus = mongoose.model('menus');
const res = require('express/lib/response');
const { debug, Console } = require('console');


module.exports = app => {

    app.post('/account/updateClientFood', async(request, response) => {


        var updateFood = await Food.find();

        var stringified = JSON.stringify(updateFood);
      
       
        response.send(updateFood);

        return;

    });
    
    app.post('/account/getAllValidRecipes', async(request, response) => {


        var res = {};

        const { rIMC} = request.body;    

        var menus = await Menus.find();
      
       
        response.send(menus);

        return;

    });

    app.post('/account/addMenuToServer', async(request, response) => {


        var res = {};

        const { info} = request.body;    
 
      
        console.log(info);
       
        //response.send(menus);

        return;

    });

    

}
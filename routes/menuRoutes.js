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
    
    app.post('/account/getAllValidMenus', async(request, response) => {


        var res = {};

        const { rIMC} = request.body;    

        var menus = await Menus.find();

        response.send(menus);

        return;

    });

    app.post('/account/addMenuToServer', async(request, response) => {


        var res = {};

        const { info} = request.body;  
        
        var parsed = JSON.parse(info);

        var newMenu = new Menus({
            name: parsed.name,
            author: parsed.author,
            aliments:parsed.aliments,
            description:parsed.description,
            score: parsed.score,
            IMCValue: parsed.IMCValue
        });

        await newMenu.save();

        res.code = 0;
        res.msg = "Menu added";
       
        response.send(res);

        return;

    });

    

}
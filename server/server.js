var HttpPort = 8000;
var V4PortReceive = 5000;
var V4PortSend = 5001;
var V4Ip = "localhost";
var ServerFolder = './ServerRoot';
var static = require('node-static');
var dgram = require('dgram');
var http = require('http');
var clients = {};
var clientFiles = new static.Server(ServerFolder);
var httpServer = http.createServer(function(request, response) {
    request.addListener('end', function () {
        clientFiles.serve(request, response);
    });
});

io = require('socket.io').listen(httpServer);
io.set('log level', 1);
httpServer.listen(HttpPort);
serverUdp = dgram.createSocket("udp4");


function UdpSend(message,Host,port){
	var client = dgram.createSocket("udp4");
	var buff = new Buffer(message);
	client.send(buff, 0, buff.length, port, Host, function(err, bytes) {
			client.close();
			});
}

io.sockets.on('connection', function (socket) {
    if(!clients[socket.id]){
    clients[socket.id] = socket; 
	var address = socket.handshake.address;
	var NewId = "NewCLient| |"+ address.address + "|" + socket.id;
		UdpSend(NewId,V4Ip,V4PortReceive)
	}	
	socket.on('message', function (msg) { 
	  // console.log(msg);
		var message = "update|"+ msg +"|"+ address.address + "|" + socket.id;
		UdpSend(message,V4Ip,V4PortReceive)
		
	});
 
	serverUdp.on("message", function (msg, rinfo) {
		var smsg = msg.toString('ascii', 0, rinfo.size);
		//console.log(smsg);
		var amsg = smsg.split(";");
		var length = amsg.length;
		
		for (var i = 0; i < length; i++) {
			var datmsg = amsg[i].split("|");
			if (clients[datmsg[3]]) {
				
				clients[datmsg[3]].emit("vvvv",amsg[i]);
				
				} else if(datmsg[3]=="broadcast"){
				
				socket.emit("vvvv",amsg[i]);
				}
			}
	
	
	});
    
	  socket.on('disconnect', function () {
	  
      var address = socket.handshake.address;
	  var DiscoId = "ClientDisconected| |"+ address.address + "|" + socket.id;
		UdpSend(DiscoId,V4Ip,V4PortReceive) 
		
		delete clients[socket.id];
		serverUdp.removeAllListeners("message");
        });


});

serverUdp.bind(V4PortSend);

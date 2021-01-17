using System;
using System.Collections.Generic;
using System.Text;

namespace ChatService01
{
    public class ChatService : ChatRoom.ChatRoomBase
    {
        private readonly Server.ChatRoom _chatroomService;

        public ChatService(Server.ChatRoom chatRoomService)
        {
            _chatroomService = chatRoomService;
        }

        public override async Task join(IAsyncStreamReader<Message> requestStream, IServerStreamWriter<Message> responseStream, ServerCallContext context)
        {
            if (!await requestStream.MoveNext()) return;

            do
            {
                _chatroomService.Join(requestStream.Current.User, responseStream);
                await _chatroomService.BroadcastMessageAsync(requestStream.Current);
            } while (await requestStream.MoveNext());

            _chatroomService.Remove(context.Peer);

        }


    }
}

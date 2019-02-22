using System.Threading.Tasks;
using AElf.Kernel;
using Grpc.Core;

namespace AElf.OS.Network.Grpc
{
    public class GrpcPeer
    {
        private readonly Channel _channel;
        private readonly PeerService.PeerServiceClient _client;
        private readonly HandshakeData _handshakeData;

        public string PeerAddress { get; }
        public string RemoteEndpoint { get; }

        private byte[] _pubKey;
        public byte[] PublicKey
        {
            get { return _pubKey ?? (_pubKey = _handshakeData?.PublicKey?.ToByteArray()); }
        }

        public GrpcPeer(Channel channel, PeerService.PeerServiceClient client, HandshakeData handshakeData, string peerAddress, string remoteEndpoint)
        {
            _channel = channel;
            _client = client;
            _handshakeData = handshakeData;

            RemoteEndpoint = remoteEndpoint;
            PeerAddress = peerAddress;
        }

        public async Task<BlockReply> RequestBlockAsync(BlockRequest blockHash)
        {
            return await _client.RequestBlockAsync(blockHash);
        }

        public async Task<BlockIdList> GetBlockIdsAsync(BlockIdsRequest idsRequest)
        {
            return await _client.RequestBlockIdsAsync(idsRequest);
        }

        public async Task AnnounceAsync(Announcement an)
        {
            await _client.AnnounceAsync(an);
        }

        public async Task SendTransactionAsync(Transaction tx)
        {
            await _client.SendTransactionAsync(tx);
        }

        public async Task StopAsync()
        {
            await _channel.ShutdownAsync();
        }
        
        public async Task SendDisconnectAsync()
        {
            await _client.DisconnectAsync(new DisconnectReason { Why = DisconnectReason.Types.Reason.Shutdown });
        }
    }
}
using ConnectFour.Models;

namespace ConnectFour.Services.Interfaces;

public interface IConnectFourService
{
    Task<string> PlacePiece(DropedPiece dropPiece);
}
import type { LobbyStatusDto } from "../../apiClient/matchmaking/dtos";


interface LobbyComponentProps {
    lobbyData: LobbyStatusDto | null;
}

function LobbyComponent({ lobbyData }: LobbyComponentProps) {
  return (
    <div className="flex flex-col w-full h-full bg-[#1F2937] p-5">
      <h1 className="text-white text-2xl font-[Oswald] mb-4">Team Found!</h1>
      
      <div className="bg-[#374151] p-4 rounded-lg">
        <h2 className="text-white text-xl mb-2">Lobby: {lobbyData?.lobbyId}</h2>
        
        <div className="space-y-2">
          <h3 className="text-white text-lg">Team Members:</h3>
          {lobbyData?.members?.map((member, index) => (
            <div key={index} className="text-gray-300 p-2 bg-[#1F2937] rounded">
              {member.name || `Member ${index + 1}`}
            </div>
          ))}
        </div>
      </div>
      
      {/* Add more lobby-specific UI elements */}
    </div>
  );
}

export default LobbyComponent;
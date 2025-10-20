import Button from "../shared/Button";
import UserAvatar from "../shared/UserAvatar";

interface UserCardProps {
    userName: string;
    userRole: string;
}

function UserCard({ userName, userRole }: UserCardProps) {
    return (
        <div className="flex flex-col items-center justify-start text-center p-3 w-full h-full">
            <UserAvatar userName={userName} />
            <h1 className="text-white text-lg font-semibold mt-1 mb-1">{userName}</h1>
            <Button className="w-48" text={userRole} isSelected={true} isDisabled={true}/>
        </div>
    )
}

export default UserCard;
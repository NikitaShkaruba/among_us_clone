rm -rf AmongUsCloneClient/Assets/Shared # We need to delete all files, because the client may have excess files, and they may falsely stay
cp -r AmongUsCloneServer/Assets/Shared AmongUsCloneClient/Assets
echo "Synchronized shared files with the client"

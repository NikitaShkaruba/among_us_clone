// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using System.Text;
using AmongUsClone.Shared.Game.PlayerLogic;
using AmongUsClone.Shared.Snapshots;
using UnityEngine;

namespace AmongUsClone.Shared.Networking
{
    /**
     * Network packet which holds data to exchange between a client and a server
     */
    public class Packet : IDisposable
    {
        private const int ReadPositionBeginning = 0;

        // Maybe it would be good to refactor those two buffers into one
        private List<byte> buffer;
        private byte[] readableBuffer;

        private int readPosition;
        private bool disposed;

        public Packet()
        {
            buffer = new List<byte>();
            readPosition = ReadPositionBeginning;
        }

        /**
         * Creates a new packet with a given dataPacketType. Used for sending
         */
        public Packet(int dataPacketType) : this()
        {
            Write(dataPacketType);
        }

        /**
         * Creates a packet from which data can be read. Used for receiving
         */
        public Packet(byte[] data) : this()
        {
            WriteBytesAndPrepareToRead(data);
        }

        #region Write Data

        /**
         * Adds a byte to the packet
         */
        public void Write(byte value)
        {
            buffer.Add(value);
        }

        /**
         * Adds an array of bytes to the packet
         */
        public void Write(byte[] value)
        {
            buffer.AddRange(value);
        }

        /**
         * Adds a short to the packet
         */
        public void Write(short value)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
        }

        /**
         * Adds an int to the packet
         */
        public void Write(int value)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
        }

        /**
         * Adds a long to the packet
         */
        public void Write(long value)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
        }

        /**
         * Adds a float to the packet
         */
        public void Write(float value)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
        }

        /**
         * Adds a bool to the packet
         */
        public void Write(bool value)
        {
            buffer.AddRange(BitConverter.GetBytes(value));
        }

        /**
         * Adds a string to the packet
         */
        public void Write(string value)
        {
            Write(value.Length);
            buffer.AddRange(Encoding.ASCII.GetBytes(value));
        }

        public void Write(Vector2 value)
        {
            Write(value.x);
            Write(value.y);
        }

        public void Write(PlayerInput playerInput)
        {
            Write(playerInput.id);

            foreach (bool inputValue in playerInput.SerializeValues())
            {
                Write(inputValue);
            }
        }

        public void Write(ClientGameSnapshot clientGameSnapshot)
        {
            Write(clientGameSnapshot.id);
            Write(clientGameSnapshot.yourLastProcessedInputId);

            Write(clientGameSnapshot.playersInfo.Count);
            foreach (SnapshotPlayerInfo snapshotPlayerInfo in clientGameSnapshot.playersInfo.Values)
            {
                Write(snapshotPlayerInfo.id);
                Write(snapshotPlayerInfo.position);
                Write(snapshotPlayerInfo.input);
            }

            Write(clientGameSnapshot.adminPanelPositions.Count);
            foreach (KeyValuePair<int, int> pair in clientGameSnapshot.adminPanelPositions)
            {
                Write(pair.Key);
                Write(pair.Value);
            }
        }

        #endregion

        #region Read Data

        /**
         * Reads a byte from the packet
         */
        public byte ReadByte(bool updateReadPosition = true)
        {
            if (HasReadEverything())
            {
                throw new Exception("Could not read value of type 'byte'!");
            }

            byte value = readableBuffer[readPosition];
            if (updateReadPosition)
            {
                readPosition += sizeof(byte);
            }

            return value;
        }

        /**
         * Reads an array of bytes from the packet
         */
        public byte[] ReadBytes(int length, bool updateReadPosition = true)
        {
            if (HasReadEverything())
            {
                throw new Exception("Could not read value of type 'byte[]'!");
            }

            byte[] value = buffer.GetRange(readPosition, length).ToArray();
            if (updateReadPosition)
            {
                readPosition += sizeof(byte) * length;
            }

            return value;
        }

        /**
         * Reads a short from the packet
         */
        public short ReadShort(bool updateReadPosition = true)
        {
            if (HasReadEverything())
            {
                throw new Exception("Could not read value of type 'short'!");
            }

            short value = BitConverter.ToInt16(readableBuffer, readPosition);
            if (updateReadPosition)
            {
                readPosition += sizeof(short);
            }

            return value;
        }

        /**
         * Reads an int from the packet
         */
        public int ReadInt(bool updateReadPosition = true)
        {
            if (HasReadEverything())
            {
                throw new Exception("Could not read value of type 'int'!");
            }

            int value = BitConverter.ToInt32(readableBuffer, readPosition);
            if (updateReadPosition)
            {
                readPosition += sizeof(int);
            }

            return value;
        }

        /**
         * Reads a long from the packet
         */
        public long ReadLong(bool updateReadPosition = true)
        {
            if (HasReadEverything())
            {
                throw new Exception("Could not read value of type 'long'!");
            }

            long value = BitConverter.ToInt64(readableBuffer, readPosition);
            if (updateReadPosition)
            {
                readPosition += sizeof(long);
            }

            return value;
        }

        /**
         * Reads a float from the packet
         */
        public float ReadFloat(bool updateReadPosition = true)
        {
            if (HasReadEverything())
            {
                throw new Exception("Could not read value of type 'float'!");
            }

            float value = BitConverter.ToSingle(readableBuffer, readPosition);
            if (updateReadPosition)
            {
                readPosition += sizeof(float);
            }

            return value;
        }

        /**
        * Reads a bool from the packet
        */
        public bool ReadBool(bool updateReadPosition = true)
        {
            if (HasReadEverything())
            {
                throw new Exception("Could not read value of type 'bool'!");
            }

            bool value = BitConverter.ToBoolean(readableBuffer, readPosition);
            if (updateReadPosition)
            {
                readPosition += sizeof(bool);
            }

            return value;
        }

        /**
         * Reads a string from the packet
         */
        public string ReadString(bool updateReadPosition = true)
        {
            try
            {
                int length = ReadInt();
                string value = Encoding.ASCII.GetString(readableBuffer, readPosition, length);
                if (updateReadPosition && value.Length > 0)
                {
                    readPosition += length;
                }

                return value;
            }
            catch
            {
                throw new Exception("Could not read value of type 'string'!");
            }
        }

        /**
         * Reads a Vector2 from the packet
         */
        public Vector2 ReadVector2(bool updateReadPosition = true)
        {
            return new Vector2(ReadFloat(updateReadPosition), ReadFloat(updateReadPosition));
        }

        public PlayerInput ReadPlayerInput(bool updateReadPosition = true)
        {
            return new PlayerInput(
                ReadInt(updateReadPosition),
                ReadBool(updateReadPosition),
                ReadBool(updateReadPosition),
                ReadBool(updateReadPosition),
                ReadBool(updateReadPosition)
            );
        }

        public ClientGameSnapshot ReadClientGameSnapshot(bool updateReadPosition = true)
        {
            int snapshotId = ReadInt(updateReadPosition);
            int lastProcessedInputId = ReadInt(updateReadPosition);

            Dictionary<int, SnapshotPlayerInfo> snapshotPlayerInfos = new Dictionary<int, SnapshotPlayerInfo>();
            int snapshotPlayersAmount = ReadInt(updateReadPosition);
            for (int snapshotPlayerIndex = 0; snapshotPlayerIndex < snapshotPlayersAmount; snapshotPlayerIndex++)
            {
                int playerId = ReadInt(updateReadPosition);
                Vector2 playerPosition = ReadVector2(updateReadPosition);
                PlayerInput playerInput = ReadPlayerInput(updateReadPosition);

                snapshotPlayerInfos[playerId] = new SnapshotPlayerInfo(playerId, playerPosition, playerInput);
            }

            Dictionary<int, int> adminPanelInfo = new Dictionary<int, int>();
            int adminPanelInfoLength = ReadInt(updateReadPosition);
            for (int adminPanelInfoPiece = 0; adminPanelInfoPiece < adminPanelInfoLength; adminPanelInfoPiece++)
            {
                int roomId = ReadInt(updateReadPosition);
                int playersAmount = ReadInt(updateReadPosition);

                adminPanelInfo[roomId] = playersAmount;
            }

            return new ClientGameSnapshot(snapshotId, lastProcessedInputId, snapshotPlayerInfos, adminPanelInfo);
        }

        #endregion

        #region Functions

        /**
         * Sets the packet's content and prepares it to be read
         */
        public void WriteBytesAndPrepareToRead(byte[] bytes)
        {
            Write(bytes);
            readableBuffer = buffer.ToArray();
        }

        /**
         * Inserts the length of the packet's content at the start of the buffer
         */
        public void WriteLength()
        {
            buffer.InsertRange(0, BitConverter.GetBytes(buffer.Count));
        }

        /**
         * Inserts the given int at the start of the buffer
         */
        public void InsertInt(int value)
        {
            buffer.InsertRange(0, BitConverter.GetBytes(value));
        }

        /**
         * Gets the packet's content in array form
         */
        public byte[] ToArray()
        {
            readableBuffer = buffer.ToArray();
            return readableBuffer;
        }

        /**
         * Gets the length of the packet's content
         */
        public int GetLength()
        {
            return buffer.Count;
        }

        /**
         * Gets the length of the unread data contained in the packet
         */
        public int GetUnreadLength()
        {
            return GetLength() - readPosition;
        }

        private bool HasReadEverything()
        {
            return buffer.Count <= readPosition;
        }

        /**
         * Resets the packet instance to allow it to be reused.
         */
        public void Reset(bool fully = true)
        {
            if (fully)
            {
                buffer.Clear();
                readableBuffer = null;
                readPosition = ReadPositionBeginning;
            }
            else
            {
                // Till this point we've only read a packet length (which is int) - so we need to remove, so that future network data can be added
                readPosition -= sizeof(int);
            }
        }

        #endregion

        #region Disposing

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                buffer = null;
                readableBuffer = null;
                readPosition = ReadPositionBeginning;
            }

            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}

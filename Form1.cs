using System;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace NamaProyekAnda
{
    public partial class Form1 : Form
    {
        byte[] kei = new byte[16];
        byte[] iv = new byte[16];
        Bitmap Ori = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Uruskey()
        {
            string KK = textBoxkey.Text.PadRight(32, '0');
            kei = Encoding.UTF8.GetBytes(KK.Substring(0, 16));
            iv = Encoding.UTF8.GetBytes(KK.Substring(0, 16));
        }

        private void buttonPilih_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Ori = new Bitmap(ofd.FileName);
                pictureBoxori.Image = Ori;
            }
        }

        private void buttonEnkripsi_Click(object sender, EventArgs e)
        {
            Uruskey();

            if (Ori == null)
            {
                MessageBox.Show("Pilih gambar terlebih dahulu.");
                return;
            }

            // Sisipkan teks ke dalam citra menggunakan LSB
            string message = "Teks rahasia";
            Bitmap stegoImage = EmbedText(message, Ori);

            // Enkripsi citra yang telah dimodifikasi dan simpan sebagai file citra
            SaveFileDialog svd = new SaveFileDialog();
            if (svd.ShowDialog() == DialogResult.OK)
            {
                EncryptImageAndSave(stegoImage, svd.FileName, kei, iv);
                MessageBox.Show("Enkripsi selesai.");
            }
        }

        private void buttonDekripsi_Click(object sender, EventArgs e)
        {
            Uruskey();

            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Bitmap decryptedBitmap = DecryptImageFromFile(ofd.FileName, kei, iv);
                pictureBoxori.Image = decryptedBitmap;

                // Ekstrak teks dari citra yang didekripsi
                string extractedMessage = ExtractText(decryptedBitmap);
                MessageBox.Show("Pesan yang disembunyikan: " + extractedMessage);
            }
        }

        private Bitmap EmbedText(string text, Bitmap bmp)
        {
            byte[] textBytes = Encoding.UTF8.GetBytes(text);
            int byteIndex = 0;
            int bitIndex = 0;

            for (int i = 0; i < bmp.Height; i++)
            {
                for (int j = 0; j < bmp.Width; j++)
                {
                    Color pixel = bmp.GetPixel(j, i);
                    byte r = pixel.R;
                    byte g = pixel.G;
                    byte b = pixel.B;

                    if (byteIndex < textBytes.Length)
                    {
                        byte bit = (byte)((textBytes[byteIndex] >> (7 - bitIndex)) & 1);
                        b = (byte)((b & 0xFE) | bit);

                        bitIndex++;
                        if (bitIndex == 8)
                        {
                            bitIndex = 0;
                            byteIndex++;
                        }
                    }

                    bmp.SetPixel(j, i, Color.FromArgb(r, g, b));
                }
            }
            return bmp;
        }

        private string ExtractText(Bitmap bmp)
        {
            byte[] textBytes = new byte[bmp.Width * bmp.Height / 8];
            int byteIndex = 0;
            int bitIndex = 0;

            for (int i = 0; i < bmp.Height; i++)
            {
                for (int j = 0; j < bmp.Width; j++)
                {
                    Color pixel = bmp.GetPixel(j, i);
                    byte b = pixel.B;

                    textBytes[byteIndex] = (byte)((textBytes[byteIndex] << 1) | (b & 1));

                    bitIndex++;
                    if (bitIndex == 8)
                    {
                        bitIndex = 0;
                        byteIndex++;
                        if (byteIndex == textBytes.Length)
                        {
                            break;
                        }
                    }
                }
                if (byteIndex == textBytes.Length)
                {
                    break;
                }
            }

            return Encoding.UTF8.GetString(textBytes).TrimEnd('\0');
        }

        private void EncryptImageAndSave(Bitmap bmp, string filePath, byte[] key, byte[] iv)
        {
            // Convert the bitmap to byte array
            byte[] imageBytes = BitmapToByteArray(bmp);

            // Encrypt the byte array
            byte[] encryptedBytes = AesImageEncryption.EncryptImage(imageBytes, key, iv);

            // Convert the encrypted byte array back to a bitmap and save
            Bitmap encryptedBitmap = ByteArrayToBitmap(encryptedBytes, bmp.Width, bmp.Height);
            encryptedBitmap.Save(filePath, System.Drawing.Imaging.ImageFormat.Bmp);
        }

        private Bitmap DecryptImageFromFile(string filePath, byte[] key, byte[] iv)
        {
            // Load the encrypted bitmap
            Bitmap encryptedBitmap = new Bitmap(filePath);

            // Convert the bitmap to byte array
            byte[] encryptedBytes = BitmapToByteArray(encryptedBitmap);

            // Decrypt the byte array
            byte[] decryptedBytes = AesImageEncryption.DecryptImage(encryptedBytes, key, iv);

            // Convert the decrypted byte array back to a bitmap
            return ByteArrayToBitmap(decryptedBytes, encryptedBitmap.Width, encryptedBitmap.Height);
        }

        private byte[] BitmapToByteArray(Bitmap bmp)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                return ms.ToArray();
            }
        }

        private Bitmap ByteArrayToBitmap(byte[] byteArray, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);

            int byteIndex = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (byteIndex + 3 <= byteArray.Length)
                    {
                        Color pixelColor = Color.FromArgb(byteArray[byteIndex], byteArray[byteIndex + 1], byteArray[byteIndex + 2]);
                        bmp.SetPixel(x, y, pixelColor);
                        byteIndex += 3;
                    }
                }
            }

            return bmp;
        }
    }

    public class AesImageEncryption
    {
        public static byte[] EncryptImage(byte[] imageBytes, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(imageBytes, 0, imageBytes.Length);
                        csEncrypt.FlushFinalBlock();
                    }

                    return msEncrypt.ToArray();
                }
            }
        }

        public static byte[] DecryptImage(byte[] encryptedImageBytes, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(encryptedImageBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            csDecrypt.CopyTo(ms);
                            return ms.ToArray();
                        }
                    }
                }
            }
        }
    }
}

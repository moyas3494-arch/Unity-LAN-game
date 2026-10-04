using Unity.Netcode;
using UnityEngine;

public class NetworkUI : MonoBehaviour
{
    void OnGUI()
    {
        // SEGURIDAD: Si por alguna razón NetworkManager no existe en la escena todavía, no dibuja nada para evitar el error.
        if (NetworkManager.Singleton == null)
        {
            GUILayout.BeginArea(new Rect(20, 20, 300, 50));
            GUILayout.Label("<b>Error:</b> Falta el NetworkManager en la escena.");
            GUILayout.EndArea();
            return;
        }

        // Dibuja una caja pequeña en la esquina superior izquierda de la pantalla
        GUILayout.BeginArea(new Rect(20, 20, 250, 150));

        // Si no estamos conectados ni como servidor/host ni como cliente, mostrar botones de inicio
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            if (GUILayout.Button("Host (Servidor + Jugador)", GUILayout.Height(40)))
            {
                NetworkManager.Singleton.StartHost();
            }

            if (GUILayout.Button("Cliente", GUILayout.Height(40)))
            {
                NetworkManager.Singleton.StartClient();
            }
        }
        else
        {
            // Mostrar estado actual una vez conectados
            string mode = NetworkManager.Singleton.IsHost ? "Host" : "Cliente";
            GUILayout.Label($"<b>Estado:</b> Conectado como {mode}");

            if (GUILayout.Button("Desconectar", GUILayout.Height(30)))
            {
                NetworkManager.Singleton.Shutdown();
            }
        }

        GUILayout.EndArea();
    }
}
using UnityEngine;
using UnityEditor;
using System.IO;

public class TextFileGeneratorWindow : EditorWindow
{
    private string fileName = "GeneratedTextFile.txt"; // デフォルトのファイル名
    private string newFileName = ""; // 新しいファイル名用の変数

    // ウィンドウを開くためのメニューを作成
    [MenuItem("Tools/Text File Generator")]
    public static void ShowWindow()
    {
        // ウィンドウを表示
        GetWindow<TextFileGeneratorWindow>("Text File Generator");
    }

    // GUIを表示する
    private void OnGUI()
    {
        // 現在のファイル名表示
        GUILayout.Label("現在のファイル名:", EditorStyles.boldLabel);
        GUILayout.Label(fileName);

        // 新しいファイル名入力用のテキストフィールドを作成
        GUILayout.Label("新しいファイル名を入力してください:", EditorStyles.boldLabel);
        newFileName = EditorGUILayout.TextField("ファイル名:", newFileName);

        // ボタンを作成
        if (GUILayout.Button("Generate or Rename Text File"))
        {
            GenerateOrRenameTextFile();
        }
    }

    // テキストファイルを生成または名前を変更するメソッド
    private void GenerateOrRenameTextFile()
    {
        // 新しいファイル名が空の場合、警告を表示
        if (string.IsNullOrWhiteSpace(newFileName))
        {
            Debug.LogWarning("ファイル名を入力してください。");
            return;
        }

        // ファイルの保存パスを指定（Assetsフォルダ内）
        string path = Application.dataPath + "/" + fileName;
        string newPath = Application.dataPath + "/" + newFileName;

        // ファイルが存在しない場合は新規作成、存在する場合は名前を変更
        if (!File.Exists(newPath))
        {
            // StreamWriterでファイルを作成し、内容を書き込む
            using (StreamWriter sw = new StreamWriter(newPath))
            {
                sw.WriteLine("このテキストはエディタ拡張によって作成されました。");
                sw.WriteLine("ファイル名: " + newFileName);
            }

            // 成功メッセージを表示
            Debug.Log("テキストファイルが生成されました: " + newPath);
            fileName = newFileName; // 現在のファイル名を更新
            newFileName = "";

            // エディタに変更を通知し、プロジェクトをリフレッシュ
            AssetDatabase.Refresh();
        }
        else
        {
            Debug.LogWarning("ファイルは既に存在します: " + newPath);
        }
    }
}

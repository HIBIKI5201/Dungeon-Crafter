using UnityEngine;
using UnityEditor;
using System.IO;

public class TextFileGeneratorWindow : EditorWindow
{
    private string fileName = "GeneratedTextFile.txt"; // �f�t�H���g�̃t�@�C����
    private string newFileName = ""; // �V�����t�@�C�����p�̕ϐ�

    // �E�B���h�E���J�����߂̃��j���[���쐬
    [MenuItem("Tools/Text File Generator")]
    public static void ShowWindow()
    {
        // �E�B���h�E��\��
        GetWindow<TextFileGeneratorWindow>("Text File Generator");
    }

    // GUI��\������
    private void OnGUI()
    {
        // ���݂̃t�@�C�����\��
        GUILayout.Label("���݂̃t�@�C����:", EditorStyles.boldLabel);
        GUILayout.Label(fileName);

        // �V�����t�@�C�������͗p�̃e�L�X�g�t�B�[���h���쐬
        GUILayout.Label("�V�����t�@�C��������͂��Ă�������:", EditorStyles.boldLabel);
        newFileName = EditorGUILayout.TextField("�t�@�C����:", newFileName);

        // �{�^�����쐬
        if (GUILayout.Button("Generate or Rename Text File"))
        {
            GenerateOrRenameTextFile();
        }
    }

    // �e�L�X�g�t�@�C���𐶐��܂��͖��O��ύX���郁�\�b�h
    private void GenerateOrRenameTextFile()
    {
        // �V�����t�@�C��������̏ꍇ�A�x����\��
        if (string.IsNullOrWhiteSpace(newFileName))
        {
            Debug.LogWarning("�t�@�C��������͂��Ă��������B");
            return;
        }

        // �t�@�C���̕ۑ��p�X���w��iAssets�t�H���_���j
        string path = Application.dataPath + "/" + fileName;
        string newPath = Application.dataPath + "/" + newFileName;

        // �t�@�C�������݂��Ȃ��ꍇ�͐V�K�쐬�A���݂���ꍇ�͖��O��ύX
        if (!File.Exists(newPath))
        {
            // StreamWriter�Ńt�@�C�����쐬���A���e����������
            using (StreamWriter sw = new StreamWriter(newPath))
            {
                sw.WriteLine("���̃e�L�X�g�̓G�f�B�^�g���ɂ���č쐬����܂����B");
                sw.WriteLine("�t�@�C����: " + newFileName);
            }

            // �������b�Z�[�W��\��
            Debug.Log("�e�L�X�g�t�@�C������������܂���: " + newPath);
            fileName = newFileName; // ���݂̃t�@�C�������X�V
            newFileName = "";

            // �G�f�B�^�ɕύX��ʒm���A�v���W�F�N�g�����t���b�V��
            AssetDatabase.Refresh();
        }
        else
        {
            Debug.LogWarning("�t�@�C���͊��ɑ��݂��܂�: " + newPath);
        }
    }
}

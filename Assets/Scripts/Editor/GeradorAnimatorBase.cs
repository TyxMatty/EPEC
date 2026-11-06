using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

// Origem: Assets/Scripts/Editor/GeradorAnimatorBase.cs
// Menu EPEC > Criar Animator Base: gera Humano4Dir e Quadrupede4Dir em Assets/Animacoes/Base.
// Cada um tem Parado/Andando/Correndo (Blend Tree 2D por DirX/DirY) com clips vazios por direcao;
// o personagem cria um AnimatorOverrideController e troca esses clips pelos dele. Nao sobrescreve o que ja existe.
public static class GeradorAnimatorBase
{
    private const string Pasta = "Assets/Animacoes/Base";
    private const float LimiarAndar = 0.1f;
    // Acima do baseSpeed (5) do MovementScript e abaixo do Sprint (5 x 1.5); NPCs a 3 so andam
    private const float LimiarCorrida = 5.5f;

    private static readonly string[] Estados = { "Parado", "Andando", "Correndo" };
    private static readonly string[] NomesDirecao = { "Cima", "Baixo", "Esquerda", "Direita" };
    private static readonly Vector2[] Direcoes = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    [MenuItem("EPEC/Criar Animator Base")]
    public static void Criar()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Animacoes")) AssetDatabase.CreateFolder("Assets", "Animacoes");
        if (!AssetDatabase.IsValidFolder(Pasta)) AssetDatabase.CreateFolder("Assets/Animacoes", "Base");

        CriarController("Humano4Dir");
        CriarController("Quadrupede4Dir");
        AssetDatabase.SaveAssets();
    }

    private static void CriarController(string tipo)
    {
        string caminho = $"{Pasta}/{tipo}.controller";
        if (AssetDatabase.LoadAssetAtPath<AnimatorController>(caminho) != null)
        {
            Debug.Log($"[GeradorAnimatorBase] {caminho} ja existe; nada foi alterado.");
            return;
        }

        AnimatorController controller = AnimatorController.CreateAnimatorControllerAtPath(caminho);
        controller.AddParameter("DirX", AnimatorControllerParameterType.Float);
        controller.AddParameter("DirY", AnimatorControllerParameterType.Float);
        controller.AddParameter("Velocidade", AnimatorControllerParameterType.Float);

        var estados = new AnimatorState[Estados.Length];
        for (int i = 0; i < Estados.Length; i++)
        {
            estados[i] = controller.CreateBlendTreeInController(Estados[i], out BlendTree arvore, 0);
            arvore.blendType = BlendTreeType.SimpleDirectional2D;
            arvore.blendParameter = "DirX";
            arvore.blendParameterY = "DirY";
            for (int d = 0; d < Direcoes.Length; d++)
            {
                var clip = new AnimationClip { name = $"{tipo}_{Estados[i]}_{NomesDirecao[d]}" };
                AssetDatabase.CreateAsset(clip, $"{Pasta}/{clip.name}.anim");
                arvore.AddChild(clip, Direcoes[d]);
            }
        }

        AnimatorState parado = estados[0], andando = estados[1], correndo = estados[2];
        controller.layers[0].stateMachine.defaultState = parado;
        // A primeira transicao valida ganha: as mais fortes vem antes
        Ligar(parado, correndo, AnimatorConditionMode.Greater, LimiarCorrida);
        Ligar(parado, andando, AnimatorConditionMode.Greater, LimiarAndar);
        Ligar(andando, parado, AnimatorConditionMode.Less, LimiarAndar);
        Ligar(andando, correndo, AnimatorConditionMode.Greater, LimiarCorrida);
        Ligar(correndo, parado, AnimatorConditionMode.Less, LimiarAndar);
        Ligar(correndo, andando, AnimatorConditionMode.Less, LimiarCorrida);

        Debug.Log($"[GeradorAnimatorBase] Criado {caminho}.");
    }

    private static void Ligar(AnimatorState de, AnimatorState para, AnimatorConditionMode modo, float limiar)
    {
        AnimatorStateTransition t = de.AddTransition(para);
        t.hasExitTime = false;
        t.duration = 0f;
        t.AddCondition(modo, limiar, "Velocidade");
    }
}

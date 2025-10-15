using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("監視対象のモデル")]
    [SerializeField] private CharacterManager _characterManager;

    [Header("操作対象のビュー（UI要素）")]
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private TextMeshProUGUI _chargeText;

    // 専門家チームのメンバー
    private HealthBarPresenter _healthBarPresenter;
    private ChargeTextPresenter _chargeTextPresenter;

    private void Awake()
    {
        // 専門家たちを生成し、必要な部品（モデルとビュー）を渡す
        _healthBarPresenter = new HealthBarPresenter(_characterManager, _hpSlider);
        _chargeTextPresenter = new ChargeTextPresenter(_characterManager, _chargeText);
    }

    private void OnEnable()
    {
        // 各専門家に監視を開始させる
        _healthBarPresenter.Enable();
        _chargeTextPresenter.Enable();
    }

    private void OnDisable()
    {
        // 各専門家に監視を終了させる
        _healthBarPresenter.Disable();
        _chargeTextPresenter.Disable();
    }
}

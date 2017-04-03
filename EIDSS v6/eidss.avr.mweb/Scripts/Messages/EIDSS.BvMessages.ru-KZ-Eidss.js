var EIDSS = {
	BvMessages: {

		'bntHideSearch': 'Скрыть поиск',
		'bntShowSearch': 'Показать поиск',
		'btnClear': 'Очистить полe',
		'btnHideErrDetail': 'Скрыть детали',
		'btnSelect': 'Выбрать',
		'btnShowErrDetail': 'Показать детали',
		'btnView': 'Просмотр',
		'strSave_Id': 'Сохранить',
		'tooltipSave_Id': 'Сохранить',
		'strClose_Id': 'Закрыть',
		'strRefresh_Id': 'Обновить',
		'tooltipRefresh_Id': 'Обновить',
		'strCreate_Id': 'Создать',
		'tooltipCreate_Id': 'Создать',
		'strEdit_Id': 'Редактировать',
		'tooltipEdit_Id': 'Редактировать',
		'Confirmation': 'Подтверждение',
		'Delete Record': 'Удалить запись',
		'ErrAuthentication': 'Для выполнения запроса надо провести авторизацию пользователя.',
		'ErrDatabase': 'Произошла ошибка во время работы с базой данных.',
		'errDatabaseNotFound': 'Не удается открыть базу данных \'{0}\' на сервере \'{1}\'. Проверьте правильность имени базы данных.',
		'ErrDataValidation': 'Некоторые поля содержат неверные данные.',
		'ErrEmptyUserLogin': 'Ячейка имени пользователя не может быть пустой',
		'ErrFieldSampleIDNotFound': 'Проба не найдена.',
		'ErrFillDataset': 'Ошибка при получении данных из базы данных',
		'ErrFilterValidatioError': 'Значение фильтра для поля [{0}] не может быть пустым.',
		'errGeneralNetworkError': 'Не удается установить соединение с SQL сервером. Проверьте, что сетевое соединение установлено и попробуйте открыть эту форму снова.',
		'ErrIncorrectDatabaseVersion': 'Версия базы данных не задана или в неправильном формате. Пожалуйста, обновите свою базу данных до последней версии.',
		'errInvailidSiteID': 'Неверный номер сайта или серийный номер',
		'errInvailidSiteType': 'Неверный тип сайта или серийный номер',
		'ErrInvalidFieldFormat': 'Неправильный формат данных для поля \'{0}\'.',
		'ErrInvalidLogin': 'Неправильное имя пользователя или пароль',
		'ErrInvalidParameter': 'Параметру команды SQL присвоено неверное значение',
		'errInvalidSearchCriteria': 'Недопустимый критерий поиска.',
		'ErrLocalFieldSampleIDNotFound': 'Инд.№ пробы при отборе не найден в таблице.',
		'ErrLoginIsLocked': 'Вы произвели 3 неудачные попытки входа. Вход для данного пользователя временно заблокирован. Попробуйте войти в программу через {0} минут.',
		'ErrLowClientVersion': 'Данная версия приложения не соответствует версии базы данных. Пожалуйста, установите последнюю версию приложения.',
		'ErrLowDatabaseVersion': 'Данное приложение требует самой последней версии базы данных. Пожалуйста, обновите свою базу данных.',
		'ErrMandatoryFieldRequired': 'Поле \'{0}\' является обязательным. Вы должны ввести данные в это поле перед сохранением формы.',
		'errNoFreeLocation': 'Нет свободного места хранения',
		'ErrOldPassword': 'Старый (текущий) пароль пользователя неверен. Пароль не был изменен.',
		'Error': 'Ошибка',
		'ErrPasswordExpired': 'Время действия вашего пароля истекло. Смените пароль.',
		'ErrPasswordPolicy': 'Невозможно изменить пароль. Предлагаемое значение нового пароля не удовлетворяет требованиям длины,сложности или истории.',
		'ErrPost': 'Ошибка при записи данных в базу данных',
		'errSampleInTransfer': 'Проба "{0}" уже включена в передачу "{1}"',
		'errSQLLoginError': 'Невозможно подключиться к SQL серверу. Проверьте правильность параметров соединения SQL в сервисной таблице SQL или подлючениях к SQL серверу.',
		'ErrSqlQuery': 'Произошла ошибка во время выполнения запроса sql.',
		'errSqlServerDoesntExist': 'Не удается подключиться к SQL серверу. Проверьте, что сетевое соединение установлено, SQL сервер не остановлен и попробуйте открыть эту форму снова.',
		'errSqlServerNotFound': 'Не удается подлючиться к SQL серверу \'{0}\'. Проверьте правильность параметров соединения на закладке SQL Сервер или доступность сервера SQL.',
		'ErrStoredProcedure': 'Ошибка при выполнении хранимой процедуры',
		'ErrUndefinedStdError': 'В работе программы произошла ошибка. Пошлите информацию об этой ошибке группе разработки программного обспечения.',
		'errUnknownError': 'В приложении возникла ошибка',
		'ErrUnprocessedError': 'В работе программы произошла ошибка. Пошлите информацию об этой ошибке группе разработки программного обспечения.',
		'ErrUserNotFound': 'Введённые вами имя или пароль пользователя - не правильны.',
		'ErrWebTemporarilyUnavailableFunction': 'ErrWebTemporarilyUnavailableFunction',
		'Message': 'Сообщение',
		'msgCancel': 'Все введенная информация будет утеряна. Продолжить?',
		'msgCantDeleteRecord': 'Запись не может быть удалена.',
		'msgClearControl': 'Нажмите Ctrl-Del для очистки значения.',
		'msgConfimation': 'Подтверждение',
		'msgConfirmClearFlexForm': 'Очистить панель задач?',
		'msgConfirmClearLookup': 'Очистить поле?',
		'msgDeletePrompt': 'Запись об объекте будет удалена. Удалить запись?',
		'msgDeleteRecordPrompt': 'Запись будет удалена. Удалить запись?',
		'msgCancelPrompt': 'Вы хотите отменить все изменения и закрыть форму?',
		'msgSavePrompt': 'Вы хотите сохранить все изменения?',
		'msgOKPrompt': 'Вы хотите сохранить все изменения и закрыть форму?',
		'msgEIDSSCopyright': 'Все права защищены © 2005-2011 Black && Veatch Special Projects Corp.',
		'msgEIDSSRunning': 'Вы не можете запустить больше одной сессии EIDSS одновременно. Одна сессия программы уже запущена.',
		'msgEmptyLogin': 'Имя пользователя не задано',
		'msgMessage': 'Сообщение',
		'msgNoDeletePermission': 'У вас нет прав на удаление этого объекта',
		'msgNoFreeSpace': 'Нет свободного места в этом элементе хранилища',
		'msgNoInsertPermission': 'У вас нет прав на создание этого объекта',
		'msgNoRecordsFound': 'Не найдено ни одной записи, удовлетворяющей данному критерию поиска.',
		'msgNoSelectPermission': 'У вас нет прав просматривать эту форму.',
		'msgParameterAlreadyExists': 'Такое поле уже существует',
		'msgPasswordChanged': 'Ваш пароль успешно изменен',
		'msgPasswordNotTheSame': 'Новый и подтвержденный пароли должны совпадать.',
		'msgReasonEmpty': 'Введите причину изменения',
		'msgReplicationPrompt': 'Начать репликацию для перемещения данных на другие сайты?',
		'msgREplicationPromptCaption': 'Подтвердить репликацию данных',
		'msgWaitFormCaption': 'Подождите, пожалуйста',
		'msgFormLoading': 'Форма загружается',
		'msgWrongDiagnosis': 'Измененный/уточненный диагноз ({0}) должен отличаться от диагноза по экстренному извещению ({1}).',
		'Save data?': 'Сохранить данные?',
		'Warning': 'Предупреждение',
		'SecurityLog_EIDSS_finished_successfully': 'ЭИСНЗ успешно закрыта',
		'SecurityLog_EIDSS_started_abnormaly': 'ЭИСНЗ стартовала с ошибками',
		'SecurityLog_EIDSS_started_successfully': 'ЭИСНЗ успешно стартовала',
		'strCancel_Id': 'Отменить',
		'strChangeDiagnosisReason_msgId': 'Причина является обязательным полем.',
		'strDelete_Id': 'Удалить',
		'strOK_Id': 'OK',
		'tooltipCancel_Id': 'Отменить',
		'tooltipClose_Id': 'Закрыть',
		'tooltipDelete_Id': 'Удалить',
		'tooltipOK_Id': 'OK',
		'titleAccessionDetails': 'Детальная информация о приеме пробы',
		'titleAntibiotic': 'Антибиотики/антивирусные препараты',
		'titleContactInformation': 'Информация о контакте',
		'titleDiagnosisChange': 'Изменение диагноза',
		'titleDuplicates': 'Дубликаты',
		'titleEmployeeDetails': 'Сотрудник',
		'titleEmployeeList': 'Список сотрудников',
		'titleGeoLocation': 'Географическое местоположение',
		'titleHumanCaseList': 'Cлучаи заболевания человека',
		'titleOrganizationList': 'Список организаций',
		'titleOutbreakList': 'Вспышки заболеваний',
		'titlePersonsList': 'Список людей, зарегистрированных в системе',
		'titleFarmList': 'titleFarmList',
		'titleTestResultChange': 'Изменение результата теста',
		'titleSampleDetails': 'Дополнительная информация о пробах',
		'titleOutbreakNote': 'titleOutbreakNote',
		'errLoginMandatoryFields': 'Все поля формы обязательны к заполнению.',
		'msgAddToPreferencesPrompt': 'Выбранные записи будут добавлены к предпочтительным записям.',
		'msgRemoveFromPreferencesPrompt': 'Выбранные записи будут удалены из предпочтительных записей.',
		'strAdd_Id': 'Добавить',
		'strRemove_Id': 'Удалить',
		'titleResultSummary': 'Интерпретация результатов проведенных тестов',
		'titleVeterinaryCaseList': 'Cлучаи заболевания животных',
		'titleVSSessionList': 'titleVSSessionList',
		'titlePensideTest': 'Полевой тест',
		'titleTestDetails': 'Информация о тесте',
		'titleASSessionList': 'Сессии активного надзора',
		'titleTestResultDetails': 'Детальные результаты теста',
		'ErrObjectCantBeDeleted': 'Объект не может быть удален.',
		'titleVaccination': 'Вакцинация',
		'msgAsSessionNoCaseCreated': 'Нет положительных проб.',
		'strYes_Id': 'Да',
		'strNo_Id': 'Нет',
		'titleClinicalSigns': 'Клинические признаки',
		'LastName': 'Фамилия',
		'FirstName': 'Имя',
		'MiddleName': 'Отчество',
		'AsCampaign_GetSessionRemovalConfirmation': 'Вы действительно хотите удалить связь с выбранной сессией?',
		'titleSelectFarm': 'Выбрать ферму',
		'Info': 'Info',
		'strSearchPanelMandatoryFields_msgId': 'Заполните, пожалуйста, все обязательные поля панели поиска',
		'menuCreateAliquot': 'Создать аликвоту',
		'menuCreateDerivative': 'Создать дериват',
		'menuTransferOutSample': 'Передать пробу в другую лабораторию',
		'menuAccessionInPoorCondition': 'Принято в плохом состоянии',
		'menuAccessionInRejected': 'Отклонено',
		'menuAmendTestResult': 'Исправить результат теста',
		'menuAssignTest': 'Назначить тест',
		'titleAnimal': 'titleAnimal',
		'btShowCustomizationWindow': 'Показать окно настройки',
		'btHideCustomizationWindow': 'Скрыть окно настройки'
	}
}